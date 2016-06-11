#region copyrights

//  Copyright 2016 Marvin Piekarek
//  Barrel.cs is part of RAREGangplank.
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
using LeagueSharp.Data;
using LeagueSharp.Data.DataTypes;
using SharpDX;

#endregion

namespace RAREGangplank.Gangplank
{
    internal class Barrel : Spell
    {
        #region variables
        /// <summary>
        ///     all placed barrels
        /// </summary>
        internal static List<BarrelSlot> Barrels;

        internal static float Rotation = 28*(float) Math.PI/180;
        internal static float Multiplicator = 1.001f;

        internal static int BarrelExplosionRadius = 325;
        internal static int BarrelConnectionRadius = 660;
        #endregion

        #region constructor
        /// <summary>
        ///     Constructor for our Barrel-Class
        /// </summary>
        public Barrel() : base(SpellSlot.E, SpellEEntry.Range, TargetSelector.DamageType.Physical)
        {
            SetSkillshot(SpellEEntry.Delay/1000f, SpellEEntry.Width, SpellEEntry.MissileSpeed,
                SpellEEntry.CollisionObjects.Any(), Utilities.ConvertToSkillshotType(SpellEEntry.SpellType));

            Barrels = new List<BarrelSlot>();

            GameObject.OnCreate += GameObject_OnCreate;
            GameObject.OnDelete += GameObjectOnOnDelete;
        }
        #endregion

        #region Lambda expressions
        /// <summary>
        /// The SpellDatabaseEntry for our E Spell
        /// </summary>
        private static SpellDatabaseEntry SpellEEntry =>
            Data.Get<SpellDatabase>().Spells.Single(spell => spell.ChampionName == "Gangplank" && spell.Slot == SpellSlot.E);

        /// <summary>
        /// Gets the first or default barrel in the explosion radius (325 units)
        /// </summary>
        /// <param name="pos"> where we are searching from </param>
        /// <returns> returns a barrelSlot or null </returns>
        private BarrelSlot GetBarrelsInRange(Vector2 pos) =>
            Barrels.FirstOrDefault(x => x.InfoMinion.Distance(pos) <= BarrelExplosionRadius);

        /// <summary>
        /// Counts all barrels in the explosion range
        /// </summary>
        /// <param name="pos"> where we are searching from </param>
        /// <returns> returns the count as int </returns>
        private int CountBarrelsInRange(Vector2 pos) =>
            Barrels.Count(x => x.InfoMinion.Distance(pos) <= BarrelExplosionRadius);
        #endregion

        private static Vector2 PredPosRotate(float angle, Vector2 pos, Vector2 rotatePos) =>
            pos.RotateAroundPoint(rotatePos, angle) * Multiplicator;

        #region subscribed events
        private void GameObjectOnOnDelete(GameObject sender, EventArgs args)
        {
            if (!sender.IsMe && sender.Name == "Barrel")
            {
                // deleting barrels that got destroyed
                var id = Barrels.FindIndex(y => y.NetworkId == sender.NetworkId);
                Barrels.RemoveAt(id);
            }
        }

        private void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            if (!sender.IsMe && sender.Name == "Barrel")
            {
                // creats a new list entry with the data of the placed barrel.
                Barrels.Add(new BarrelSlot(sender.NetworkId, (Obj_AI_Minion)sender ));
            }
        }
        #endregion

        #region functions and methods
        /// <summary>
        ///     The handler for our Barrels
        /// </summary>
        /// <param name="orbMode">current orbwalk mode</param>
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
                // selecting a good target (TS choise)
                var target = TargetSelector.GetTarget(Range, TargetSelector.DamageType.Physical, false);

                if (target == null) return;
                // hyper hyper reduction radius for calculations 
                // so barrels won't stay on max range. 
                var reductionRadius = GMenu.MainMenu.Item("radiusCM").GetValue<Slider>().Value;
                // calculating the difference between me and the player
                var difference = (Range - reductionRadius) - Gangplank.Player.Distance(target);

                if (difference > 0)
                {
                    var prediction = GetPrediction(target, true);
                    var castPos = prediction.CastPosition.To2D();
                    var unitPos = prediction.UnitPosition.To2D();
                    Vector2 pos;

                    for (int i = 0; i < 3; i++)
                    {
                        if (i == 0)
                        {
                            pos = PredPosRotate(0, castPos, unitPos);
                            Cast(pos);
                            Utility.DelayAction.Add(2000, () =>
                            {
                                pos = PredPosRotate(Rotation, castPos, unitPos);
                                Cast(pos);

                                pos = PredPosRotate(-Rotation, castPos, unitPos);
                                Cast(pos);
                            });
                        }               
                        
                    }
                    
                }

            }
            else if (orbMode == Orbwalking.OrbwalkingMode.LaneClear)
            {
                // get all minions that are enemies and being in range of the E + 100 units
                // order them by their current health
                var minions = ObjectManager.Get<Obj_AI_Base>()
                    .Where(m => m.IsMinion && m.IsEnemy && m.Distance(Gangplank.Player.Position) <= Range + 100)
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
                        Cast(bestFarm.Position);
                    }
                    // otherwise we will go on looking for another good position
                    // since the since the user decided to get more than one barrel
                    else if (maxBarrels > 1 && countBarrelsOnFarm > 0)
                    {
                        // get a new position (if possible) from the already placed barrel
                        var barrel = GetBarrelsInRange(bestFarm.Position);

                        if (barrel == null)
                            return;

                        // get the minions near to our barrel (230 < x < 660)
                        // so between the explosion range and the max connection range
                        var secminions = ObjectManager.Get<Obj_AI_Base>()
                            .Where(
                                m =>
                                    m.IsMinion && m.IsEnemy && barrel.InfoMinion.Distance(m) > BarrelExplosionRadius &&
                                    barrel.InfoMinion.Distance(m) < BarrelConnectionRadius).ToList();

                        // spotting the best farm loc
                        var secFarm = GetBestLoc(secminions);

                        // checking if the farmLoc is valid for us
                        if (secFarm.Position != Vector2.Zero && Gangplank.Player.Distance(secFarm.Position) <= Range)
                        {
                            Cast(secFarm.Position);
                        }
                    }
                }
            }
        }

        internal float GetMaxCooldown()
        {
            if (Level > 0)
                return float.MaxValue;

            var cooldowns = new[] { 18f, 17f, 16f, 15f, 14f };
            var barrelsCount = 2f + Gangplank.RSpell.Level;

            return cooldowns[Level - 1] / barrelsCount;
        }

        private MinionManager.FarmLocation GetBestLoc(List<Obj_AI_Base> baseList)
        {
            if (baseList == null)
                return new MinionManager.FarmLocation(Vector2.Zero, -1);

            var positions = MinionManager.GetMinionsPredictedPositions(baseList, Delay, Width, Speed,
                    Gangplank.Player.Position, Range, false, SkillshotType.SkillshotCircle);
            // uses the predicted position to get a good circular farm spot 
            return MinionManager.GetBestCircularFarmLocation(positions, Width, Range);
             
        }
        #endregion

    }

    internal class BarrelSlot
    {
        /// <summary>
        ///     equals the barrel
        /// </summary>
        internal Obj_AI_Minion InfoMinion;

        /// <summary>
        ///     Identifier for managing
        /// </summary>
        internal long NetworkId;

        /// <summary>
        ///     Init BarrelSlot
        /// </summary>
        /// <param name="netId"> is the unique network ID of the barrel </param>
        /// <param name="objai"> is the casted object of the barrel </param>
        public BarrelSlot(long netId, Obj_AI_Minion objai)
        {
            NetworkId = netId;
            InfoMinion = objai;
        }
    }
}
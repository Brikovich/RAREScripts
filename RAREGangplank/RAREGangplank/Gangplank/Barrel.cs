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
        ///     the stacks of the ESpell
        /// </summary>
        private static int BarrelCount;

        /// <summary>
        ///     all placed barrels
        /// </summary>
        internal static List<BarrelSlot> _barrels;

        private static int BarrelExplosionRadius = 325;
        private static int BarrelConnectionRadius = 660;
        #endregion

        #region constructor
        /// <summary>
        ///     Constructor for our Barrel-Class
        /// </summary>
        public Barrel() : base(SpellSlot.E, SpellEEntry.Range, TargetSelector.DamageType.Physical)
        {
            SetSkillshot(SpellEEntry.Delay/1000f, SpellEEntry.Width, SpellEEntry.MissileSpeed,
                SpellEEntry.CollisionObjects.Any(), Utilities.ConvertToSkillshotType(SpellEEntry.SpellType));

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
        private static BarrelSlot GetBarrelsInRange(Vector2 pos) =>
            _barrels.FirstOrDefault(x => x.InfoMinion.Distance(pos) <= BarrelExplosionRadius);

        /// <summary>
        /// Counts all barrels in the explosion range
        /// </summary>
        /// <param name="pos"> where we are searching from </param>
        /// <returns> returns the count as int </returns>
        private static int CountBarrelsInRange(Vector2 pos) =>
            _barrels.Count(x => x.InfoMinion.Distance(pos) <= BarrelExplosionRadius);
        #endregion

        #region subscribed events
        private void GameObjectOnOnDelete(GameObject sender, EventArgs args)
        {
            if (!sender.IsMe && sender.Name == "Barrel")
            {
                // deleting barrels that got destroyed
                var id = _barrels.FindIndex(y => y.NetworkID == sender.NetworkId);
                _barrels.RemoveAt(id);
            }
        }

        private void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            if (!sender.IsMe && sender.Name == "Barrel")
            {
                // creats a new list entry with the data of the placed barrel.
                _barrels.Add(new BarrelSlot(sender.NetworkId, sender as Obj_AI_Minion));
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
        }

        private void PlaceBarrels(Orbwalking.OrbwalkingMode orbMode, int maxBarrels, List<Obj_AI_Base> baseList = null)
        {
            // TODO: testing this shit
            if (orbMode == Orbwalking.OrbwalkingMode.LaneClear)
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
        internal long NetworkID;

        /// <summary>
        ///     Init BarrelSlot
        /// </summary>
        /// <param name="netID"> is the unique network ID of the barrel </param>
        /// <param name="objai"> is the casted object of the barrel </param>
        public BarrelSlot(long netID, Obj_AI_Minion objai)
        {
            NetworkID = netID;
            InfoMinion = objai;
        }
    }
}
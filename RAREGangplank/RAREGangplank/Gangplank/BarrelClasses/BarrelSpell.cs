#region copyrights

//  Copyright 2016 Marvin Piekarek
//  BarrelSpell.cs is part of RAREGangplank.
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

#endregion

namespace RAREGangplank.Gangplank.BarrelClasses
{

    public class BarrelSpell : Spell
    {
        #region Fields and Constants

        internal static List<Barrel> ActiveBarrels;

        internal readonly int ConnectionRadius = 650;

        internal readonly int ExplosionRadius = 400;

        private static SpellDatabaseEntry SpellEEntry =>
            Data.Get<SpellDatabase>()
                .Spells.Single(spell => spell.ChampionName == "Gangplank" && spell.Slot == SpellSlot.E);

        #endregion

        #region Constructors

        public BarrelSpell() : base(SpellSlot.E, SpellEEntry.Range, TargetSelector.DamageType.Physical)
        {
            SetSkillshot(SpellEEntry.Delay/1000f, SpellEEntry.Width, SpellEEntry.MissileSpeed,
                SpellEEntry.CollisionObjects.Any(), Utilities.ConvertToSkillshotType(SpellEEntry.SpellType));

            GameObject.OnCreate += GameObject_OnCreate;
            GameObject.OnDelete += GameObjectOnOnDelete;
        }

        #endregion

        private void GameObjectOnOnDelete(GameObject sender, EventArgs args)
        {
            if (!sender.IsMe && sender.Name == "Barrel")
            {
                // deleting barrels that got destroyed
                var id = ActiveBarrels.FindIndex(y => y.data.NetworkId == sender.NetworkId);
                ActiveBarrels.RemoveAt(id);
            }
        }

        private void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            if (!sender.IsMe && sender.Name == "Barrel")
            {
                // creats a new list entry with the data of the placed barrel.
                ActiveBarrels.Add(new Barrel((Obj_AI_Base) sender));
            }
        }

    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using LeagueSharp.Data.DataTypes;
using LeagueSharp.Data.Enumerations;
using SharpDX;

namespace RAREGangplank.Gangplank
{
    class Barrel : Spell
    {

        /// <summary>
        /// The SpellDatabaseEntry for our E Spell
        /// </summary>
        private static SpellDatabaseEntry SpellEEntry
            =>
                LeagueSharp.Data.Data.Get<LeagueSharp.Data.DataTypes.SpellDatabase>()
                    .Spells.Single(spell => spell.ChampionName == "Gangplank" && spell.Slot == SpellSlot.E);

        /// <summary>
        /// the stacks of the ESpell
        /// </summary>
        private static int BarrelCount;

        /// <summary>
        /// all placed barrels
        /// </summary>
        internal static List<BarrelSlot> _barrels;

        private static int BarrelExplosionRadius = 325;
        private static int BarrelConnectionRadius = 660;

        /// <summary>
        /// Constructor for our Barrel-Class
        /// </summary>
        public Barrel() : base(SpellSlot.E, SpellEEntry.Range, TargetSelector.DamageType.Physical)
        {
            this.SetSkillshot(SpellEEntry.Delay, SpellEEntry.Width, SpellEEntry.MissileSpeed, 
                SpellEEntry.CollisionObjects.Any(), Utilities.ConvertToSkillshotType(SpellEEntry.SpellType));

            GameObject.OnCreate += GameObject_OnCreate;
            GameObject.OnDelete += GameObjectOnOnDelete;

        }

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

        /// <summary>
        /// The handler for our Barrels
        /// </summary>
        /// <param name="orbMode">current orbwalk mode</param>
        public void HandleBarrel(Orbwalking.OrbwalkingMode orbMode)
        {
            if (orbMode == Orbwalking.OrbwalkingMode.LaneClear && GMenu.MainMenu.Item("barrelLC").GetValue<bool>())
            {
                // get all minions that are enemies and being in range of the E + 100 units
                // order them by their current health
                var minions = ObjectManager.Get<Obj_AI_Base>()
                    .Where(m => m.IsMinion && m.IsEnemy && m.Distance(Gangplank.Player.Position) <= this.Range + 100)
                    .OrderByDescending(m => m.Health).ToList();

                this.PlaceBarrels(orbMode, minions);
            }
        }

        public void PlaceBarrels(Orbwalking.OrbwalkingMode orbMode, List<Obj_AI_Base> baseList = null)
        {
            if (orbMode == Orbwalking.OrbwalkingMode.LaneClear && baseList != null)
            {
                // TODO: getting the best position for placing the barrel
                var countCast = GMenu.MainMenu.Item("countLC").GetValue<Slider>().Value;
            }

            return 0;
        }


    }

    internal class BarrelSlot
    {

        /// <summary>
        /// Identifier for managing
        /// </summary>
        internal long NetworkID;

        /// <summary>
        /// equals the barrel
        /// </summary>
        internal Obj_AI_Minion InfoMinion;

        /// <summary>
        /// Init BarrelSlot
        /// </summary>
        /// <param name="netID"> is the unique network ID of the barrel </param>
        /// <param name="objai"> is the casted object of the barrel </param>
        public BarrelSlot(long netID, Obj_AI_Minion objai)
        {
            this.NetworkID = netID;
            this.InfoMinion = objai;
        }
    }

}

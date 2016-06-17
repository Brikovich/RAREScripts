#region copyrights

//  Copyright 2016 Marvin Piekarek
//  Gangplank.cs is part of RAREGangplank.
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
using LeagueSharp;
using LeagueSharp.Common;
using RAREGangplank.Gangplank.BarrelClasses;

#endregion

namespace RAREGangplank.Gangplank
{

    internal class Gangplank
    {
        #region Fields and Constants

        /// <summary>
        ///   Constant that holds the Championname
        /// </summary>
        private const string Heroname = "Gangplank";

        internal static Orbwalking.Orbwalker OrbW;

        internal static TargetSelector Selector;

        /// <summary>
        ///   Variable for holding the BarrelSpell
        /// </summary>
        internal static BarrelLogic BarrelLogic;

        internal static Citrus CitrusSpell;

        internal static Shot ShotSpell;

        internal static Spell RSpell;

        /// <summary>
        ///   Equals the current player with all his data
        /// </summary>
        internal static Obj_AI_Hero Player => ObjectManager.Player;

        #endregion

        /// <summary>
        ///   Loading method for initilaziation of our whole assembly
        /// </summary>
        public void Load()
        {
            if (Player.CharData.BaseSkinName != Heroname)
                return;

            // init spells
            BarrelLogic = new BarrelLogic();
            CitrusSpell = new Citrus();
            ShotSpell = new Shot();
            RSpell = new Spell(SpellSlot.R);

            // init Menu
            GMenu.Init();

            // init TargetSelector
            Selector = new TargetSelector();

            // subscribe to events
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnUpdate += Game_OnUpdate;
        }

        /// <summary>
        ///   OnUpdate Event (ticks every 10ms)
        /// </summary>
        /// <param name="args"></param>
        private void Game_OnUpdate(EventArgs args)
        {
            switch (OrbW.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.LaneClear:
                    BarrelLogic.HandleBarrel(OrbW.ActiveMode);
                    ShotSpell.HandleSpell(OrbW.ActiveMode);
                    break;
                case Orbwalking.OrbwalkingMode.Combo:
                    BarrelLogic.HandleBarrel(OrbW.ActiveMode);
                    ShotSpell.HandleSpell(OrbW.ActiveMode);
                    break;
            }
        }

        /// <summary>
        ///   The Drawing on Draw event for every drawing we make.
        ///   Has lowered refresh rates than GameOnUpdate
        /// </summary>
        /// <param name="args"></param>
        private void Drawing_OnDraw(EventArgs args)
        {
            var drawQ = GMenu.MainMenu.Item("drawQ").GetValue<Circle>();
            var drawE = GMenu.MainMenu.Item("drawE").GetValue<Circle>();

            if (ShotSpell.Level >= 1 && drawQ.Active)
                Render.Circle.DrawCircle(Player.Position, ShotSpell.Range, drawQ.Color);

            if (BarrelLogic.barrel.Level >= 1 && drawE.Active)
                Render.Circle.DrawCircle(Player.Position, BarrelLogic.barrel.Range, drawE.Color);
        }

        private float GetComboDamage(Obj_AI_Base target)
        {
            var damage = Player.GetComboDamage(target, GetSpellCombo());
            return (float) (damage + Player.GetAutoAttackDamage(target));
        }

        private List<SpellSlot> GetSpellCombo()
        {
            var spelllist = new List<SpellSlot>();

            if (ShotSpell.IsReady())
                spelllist.Add(ShotSpell.Slot);
            if (BarrelLogic.barrel.IsReady())
                spelllist.Add(BarrelLogic.barrel.Slot);
            if (CitrusSpell.IsReady())
                spelllist.Add(CitrusSpell.Slot);

            return spelllist;
        }

    }

}
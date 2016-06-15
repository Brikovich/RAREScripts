#region copyrights

//  Copyright 2016 Marvin Piekarek
//  GMenu.cs is part of RAREGangplank.
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

using System.Drawing;
using LeagueSharp.Common;

#endregion

namespace RAREGangplank
{

    internal static class GMenu
    {
        #region Fields and Constants

        public static Menu MainMenu;

        #endregion

        public static void Init()
        {
            MainMenu = new Menu("RAREGangplank", "raregangplank", true);

            // init Orbwalker
            Gangplank.Gangplank.OrbW =
                new Orbwalking.Orbwalker(MainMenu.AddSubMenu(new Menu(":: Orbwalker", "orbwalker")));

            var comboMenu = MainMenu.AddSubMenu(new Menu(":: Shot", "Shot"));
            comboMenu.AddItem(new MenuItem("shotCM", "Use in Combo").SetValue(true));
            comboMenu.AddItem(new MenuItem("shotLC", "Use in LaneClear").SetValue(true));

            var harassMenu = MainMenu.AddSubMenu(new Menu(":: Barrel", "Barrel"));
            harassMenu.AddItem(new MenuItem("barrelCM", "Use in Combo").SetValue(true));
            harassMenu.AddItem(new MenuItem("radiusCM", "reduce connectionradius").SetValue(new Slider(50, 0, 150)));
            harassMenu.AddItem(new MenuItem("autoBarrel", "Automatically passive Barrel").SetValue(true));

            harassMenu.AddItem(new MenuItem("barrelLC", "Use in LaneClear").SetValue(true));
            harassMenu.AddItem(new MenuItem("countLC", "How many barrels to use").SetValue(new Slider(1, 1, 4)));
            harassMenu.AddItem(new MenuItem("hitLC", "Hitting minions").SetValue(new Slider(4, 1, 10)));


            var citrusMenu = MainMenu.AddSubMenu(new Menu(":: Citrus", "Citrus"));
            citrusMenu.AddItem(new MenuItem("lifeCitrus", "Min Health %").SetValue(new Slider(70)));

            var drawMenu = MainMenu.AddSubMenu(new Menu(":: Drawing", "Drawing"));
            drawMenu.AddItem(
                new MenuItem("drawQ", "Draw Q range").SetValue(new Circle(true, Color.FromArgb(125, 0, 255, 0))));
            drawMenu.AddItem(
                new MenuItem("drawE", "Draw E range").SetValue(new Circle(false, Color.FromArgb(125, 0, 0, 255))));
            var dmgAfterComboItem = new MenuItem("DamageAfterCombo", "Draw Combo Damage").SetValue(true);
            drawMenu.AddItem(dmgAfterComboItem);

            MainMenu.AddToMainMenu();
        }

    }

}
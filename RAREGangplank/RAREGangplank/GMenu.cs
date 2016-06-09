#region copyrights

//  Copyright 2016 Marvin Piekarek
//  MenuTwisted.cs is part of RARETwistedFate.
//  RARETwistedFate is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//  RARETwistedFate is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//  You should have received a copy of the GNU General Public License
//  along with RARETwistedFate. If not, see <http://www.gnu.org/licenses/>.

#endregion

#region usages

using System;
using LeagueSharp.Common;


#endregion

namespace RAREGangplank
{
    internal static class GMenu
    {
        public static Menu MainMenu; 

        public static void Init()
        {
            MainMenu = new Menu("RAREGangplank", "raregangplank", true);

            Menu comboMenu = MainMenu.AddSubMenu(new Menu("Shot", "Shot"));
            comboMenu.AddItem(new MenuItem("comboQ", "Use Q").SetValue(true));
            comboMenu.AddItem(new MenuItem("comboW", "Use W").SetValue(true));
            comboMenu.AddItem(new MenuItem("comboE", "Use E").SetValue(true));
            comboMenu.AddItem(new MenuItem("comboR", "Use R").SetValue(true));
            comboMenu.AddItem(new MenuItem("comboROnlyUserInitiate", "Use R only if user initiated").SetValue(false));


            var harassMenu = MainMenu.AddSubMenu(new Menu("Harass", "Harass"));
            harassMenu.AddItem(new MenuItem("harassQ", "Use Q").SetValue(true));
            harassMenu.AddItem(new MenuItem("harassE", "Use E").SetValue(true));
            harassMenu.AddItem(new MenuItem("harassPercent", "Skills until Mana %").SetValue(new Slider(20)));

            var citrusMenu = MainMenu.AddSubMenu(new Menu("Citrus", "Citrus"));
            citrusMenu.AddItem(new MenuItem("lifeCitrus", "Min Health %").SetValue(new Slider(70)));

            var drawMenu = MainMenu.AddSubMenu(new Menu("Drawing", "Drawing"));
            drawMenu.AddItem(new MenuItem("drawQ", "Draw Q range").SetValue(new Circle(true, System.Drawing.Color.FromArgb(125, 0, 255, 0))));
            drawMenu.AddItem(new MenuItem("drawE", "Draw E range").SetValue(new Circle(true, System.Drawing.Color.FromArgb(125, 254, 13, 113))));
            drawMenu.AddItem(new MenuItem("drawW", "Draw W range").SetValue(new Circle(false, System.Drawing.Color.FromArgb(125, 0, 0, 255))));
            var dmgAfterComboItem = new MenuItem("DamageAfterCombo", "Draw Combo Damage").SetValue(true);
            drawMenu.AddItem(dmgAfterComboItem);

            MainMenu.AddToMainMenu();
        }

    }
}

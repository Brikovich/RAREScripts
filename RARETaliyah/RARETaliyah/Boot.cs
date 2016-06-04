#region copyrights

//  Copyright 2016 Marvin Piekarek
//  Boot.cs is part of RARETaliyah.
//  RARETaliyah is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//  RARETaliyah is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//  You should have received a copy of the GNU General Public License
//  along with SFXUtility. If not, see <http://www.gnu.org/licenses/>.

#endregion

#region usages

using LeagueSharp;

#endregion

namespace RARETaliyah
{

    /// <summary>
    ///     the Class boot is need for initialzing <see cref="Boot" />
    /// </summary>
    internal class Boot
    {
        /// <summary>
        ///     Constructor for <see cref="Boot" />.
        /// </summary>
        public Boot()
        {
            OnLoad();
        }

        /// <summary>
        ///     The OnLoad method is initializing the whole champion
        /// </summary>
        private void OnLoad()
        {
            var taliyah = new Taliyah.Taliyah();
            Game.OnUpdate += taliyah.Game_OnUpdate;
            Drawing.OnDraw += taliyah.Drawing_OnDraw;
        }
    }
}
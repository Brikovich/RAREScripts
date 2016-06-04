#region copyrights

//  Copyright 2016 Marvin Piekarek
//  Passive.cs is part of RARETaliyah.
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

using System.Collections.Generic;
using LeagueSharp.SDK;
using SharpDX;

#endregion

namespace RARETaliyah.Taliyah
{
    internal class Passive
    {
        /// <summary>
        ///     Constuctor should be called at first.
        /// </summary>
        /// <param name="range">a proper integer that specifes the passive range</param>
        public Passive(int radius)
        {
            Range = radius*2;
        }

        internal bool IsActive => GameObjects.Player.HasBuff(""); //TODO: Add the proper buffname.
        internal int Range { get; private set; }

        /// <summary>
        ///     GetAllCurrentPassives in Range of the position you gave.
        /// </summary>
        /// <param name="range">an integer value that provides a proper range</param>
        /// <param name="position"> </param>
        /// <returns></returns>
        internal List<Passiv> GetCurrentPassives(Vector2 position, int range)
        {
            //Method to catch up current passive in range on this position.
            return new List<Passiv>();
        }

        /// <summary>
        ///     class for storing Taliyah's Passivs.
        /// </summary>
        internal class Passiv
        {
            private Vector2 _position;
            private int _range;

            public Passiv(int range, Vector2 position)
            {
                _range = range;
                _position = position;
            }
        }
    }
}
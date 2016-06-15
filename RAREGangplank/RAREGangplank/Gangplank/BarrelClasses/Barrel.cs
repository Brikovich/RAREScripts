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

using LeagueSharp;

#endregion

namespace RAREGangplank.Gangplank.BarrelClasses
{

    internal class Barrel
    {
        #region Fields and Constants

        internal Obj_AI_Base Data;

        #endregion

        #region Constructors

        public Barrel(Obj_AI_Base mybase)
        {
            Data = mybase;
        }

        #endregion
    }

}
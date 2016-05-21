
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LeagueSharp;
using LeagueSharp.SDK;
using SharpDX;

namespace RARETaliyah.Taliyah
{
    internal class Passive
    {

        internal bool IsActive => GameObjects.Player.HasBuff(""); //TODO: Add the proper buffname.
        internal int Range { get; private set; }

        /// <summary>
        /// Constuctor should be called at first. 
        /// </summary>
        /// <param name="range">a proper integer that specifes the passive range</param>
        public Passive(int radius)
        {
            this.Range = radius * 2;
        }

        /// <summary>
        /// class for storing Taliyah's Passivs.
        /// </summary>
        internal class Passiv
        {
            int Range;
            Vector2 Position;

            public Passiv(int range, Vector2 position)
            {
                this.Range = range;
                this.Position = position;
            }
        }

        /// <summary>
        /// GetAllCurrentPassives in Range of the position you gave.
        /// </summary>
        /// <param name="range">an integer value that provides a proper range</param>
        /// <param name="position"> </param>
        /// <returns></returns>
        internal List<Passiv> GetCurrentPassives(Vector2 position, int range)
        {
            //Method to catch up current passive in range on this position.
            return new List<Passiv>();
        }

    }
}

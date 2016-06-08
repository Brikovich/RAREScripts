using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace RAREGangplank.Gangplank
{
    class Gangplank
    {

        internal static Obj_AI_Hero Player => ObjectManager.Player;
        private const string HERONAME = "Gangplank";

        private static Barrel barrelSpell;

        public static void Load()
        {
            if (Player.CharData.BaseSkinName != HERONAME)
                return;

            barrelSpell = new Barrel();

            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnUpdate += Game_OnUpdate; 
            
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            // implement champion logic :klappa:

        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            // implement all skills with drawings :))
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.SDK.UI;
using RAREAIO.Plugins;

namespace RAREAIO
{
    class Bootstrap
    {

        public static Menu wardmenu, telemenu, DamageMenu, WayMenu, CloneMenu, ClockMenu, SpellMenu, AutoMenu, GameMenu, JungleMenu, Trinket, LevelMenu, CSMenu;

        internal static Clocktime clock = new Clocktime();

        public static void MainInit()
        {

            MeunInit();

            // wards

            // teleport detector

            // Damage Indicator

            // Waypoints (ways of the enemies)

            // Clone

            // clocktime
            clock.Init();

            // spelltracker (incl. summoners) Cooldowns

            // Attackranges

            // Game on Start && End

            // Jungletimer

            // AutoBuy Trinket. 

            // AutoUltLeveler

            // CSCounter


            Game.OnUpdate += Game_OnUpdate;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (!Utilities.MainMenu["CLOCK"]["ON"] && clock.IsRunning)
            {
                clock.Disable();
            }
            else if(Utilities.MainMenu["CLOCK"]["ON"] && !clock.IsRunning)
            {
                clock.Enable();
            }
        }

        public static void MeunInit()
        {
            wardmenu = Utilities.MainMenu.Add(new Menu("WARD", "Wards"));
            {
                wardmenu.Separator("Hi");
            }

            telemenu = Utilities.MainMenu.Add(new Menu("TELEPORT", "Teleport"));
            {
                telemenu.Separator("Hi");
            }

            DamageMenu = Utilities.MainMenu.Add(new Menu("DAMAGE", "DamageIndicator"));
            {
                DamageMenu.Separator("Hi");
            }

            WayMenu = Utilities.MainMenu.Add(new Menu("WAYPOINT", "Waypoint"));
            {
                WayMenu.Separator("Hi");
            }

            CloneMenu = Utilities.MainMenu.Add(new Menu("CLONE", "Clones"));
            {
                CloneMenu.Separator("Hi");
            }

            ClockMenu = Utilities.MainMenu.Add(new Menu("CLOCK", "Clock"));
            {
                ClockMenu.Bool("ON", "Turn on", false);
            }

            SpellMenu = Utilities.MainMenu.Add(new Menu("SPELL", "SpellTracker"));
            {
                SpellMenu.Separator("Hi");
            }

            AutoMenu = Utilities.MainMenu.Add(new Menu("AUTO", "Autoattacks"));
            {
                AutoMenu.Separator("Hi");
            }

            GameMenu = Utilities.MainMenu.Add(new Menu("GAME", "GameActions"));
            {
                GameMenu.Separator("Hi");
            }

            JungleMenu = Utilities.MainMenu.Add(new Menu("JUNGLE", "JungleTimer"));
            {
                JungleMenu.Separator("Hi");
            }

            Trinket = Utilities.MainMenu.Add(new Menu("TRINKET", "TrinketBuy"));
            {
                Trinket.Separator("Hi");
            }

            LevelMenu = Utilities.MainMenu.Add(new Menu("LEVEL", "AutoLeveler"));
            {
                LevelMenu.Separator("Hi");
            }

            CSMenu = Utilities.MainMenu.Add(new Menu("CS", "CSCounter"));
            {
                CSMenu.Separator("Hi");
            }
        }
    }
}

#region copyright

// Copyright (c) KyonLeague 2016
// If you want to copy parts of the code, please inform the author and give appropiate credits
// File: Utilities.cs
// Author: KyonLeague
// Contact: "cryz3rx" on Skype 

#endregion

#region usage

using System;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.Core.UI.IMenu;

#endregion

namespace RAREKarthus
{
    internal class Utilities
    {

        internal static Menu MainMenu;
        internal static Obj_AI_Hero Player;
        internal static Spell Q, W, E, R;
        internal static SpellSlot Flash, Ignite;
        internal const int FlashRange = 425, IgniteRange = 600;

        public static void PrintChat(string text)
        {
            Game.PrintChat("rareKarthus => {0}", text);
        }

        public static void UpdateCheck()
        {
            try
            {
                using (var web = new WebClient())
                {
                    var source = "";

                    if (source == "") return;

                    var rawFile = web.DownloadString("");
                    var checkFile =
                        new Regex(@"\[assembly\: AssemblyVersion\(""(\d{1,})\.(\d{1,})\.(\d{1,})\.(\d{1,})""\)\]").Match
                            (rawFile);

                    if (!checkFile.Success)
                    {
                        return;
                    }

                    var gitVersion =
                        new Version(
                            $"{checkFile.Groups[1]}.{checkFile.Groups[2]}.{checkFile.Groups[3]}.{checkFile.Groups[4]}");

                    if (gitVersion > Assembly.GetExecutingAssembly().GetName().Version)
                    {
                        PrintChat("Outdated! Newest Version: " + gitVersion);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static void InitMenu()
        {
            MainMenu = new Menu("rarekarthus", "rareKarthus", true, Player.ChampionName).Attach();
            MainMenu.Separator("We love LeagueSharp.");
            MainMenu.Separator("Developer: @Kyon");
        }
    }
}
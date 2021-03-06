﻿#region copyright

// Copyright (c) KyonLeague 2016
// If you want to copy parts of the code, please inform the author and give appropiate credits
// File: Program.cs
// Author: KyonLeague
// Contact: "cryz3rx" on Skype 

#endregion

#region usage

using System;
using LeagueSharp.SDK;
using RAREKarthus.ChampionModes;

#endregion

namespace RAREKarthus
{
    internal class Program
    {

        private static void Main(string[] args)
        {
            Bootstrap.Init(new string[1]);
            Events.OnLoad += Events_OnLoad;
        }

        private static void Events_OnLoad(object sender, EventArgs e)
        {
            Utilities.Player = GameObjects.Player;
            Utilities.InitMenu();
            Utilities.UpdateCheck();

            if (Utilities.Player.CharData.BaseSkinName == "Karthus")
            {
                var champion = new Karthus();
                champion.Init();
                Utilities.PrintChat("Karthus Initialized.");
            }
            
        }
    }
}
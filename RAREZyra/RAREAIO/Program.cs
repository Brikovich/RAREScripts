#region copyright

// Copyright (c) KyonLeague 2016
// If you want to copy parts of the code, please inform the author and give appropiate credits
// File: Program.cs
// Author: KyonLeague
// Contact: "cryz3rx" on Skype 

#endregion

#region usage

using System;
using LeagueSharp.SDK;

#endregion

namespace RAREAIO
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Events.OnLoad += Events_OnLoad;
        }

        private static void Events_OnLoad(object sender, EventArgs e)
        {
            Utilities.Player = GameObjects.Player;
            Utilities.InitMenu();
            Utilities.UpdateCheck();
            
            Bootstrap.MainInit();

        }
    }
}
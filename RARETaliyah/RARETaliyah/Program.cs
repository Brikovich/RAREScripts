
using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.Enumerations;
using RARETaliyah.Taliyah;

namespace RARETaliyah
{
    class Program
    {
        static void Main(string[] args)
        {
            Events.OnLoad += Events_OnLoad;
        }

        private static void Events_OnLoad(object sender, System.EventArgs e)
        {

            Bootstrap.Init();
            if(GameObjects.Player.CharData.BaseSkinName != "Taliyah") return;
            var boot = new Boot();

        }

    }
}

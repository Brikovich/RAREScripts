using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.SDK.UI;
using SharpDX;
using Color = System.Drawing.Color;

namespace RAREAIO.Plugins
{
    class Clocktime
    {

        public void Init()
        {
            OnLoad();
            Utilities.PrintChat("Clock activated.");
        }

        protected void OnLoad()
        {
            try
            {
                Bootstrap.ClockMenu.Bool("ShowTime", "Show time");
                Drawing.OnDraw += OnDrawingDraw;
            }
            catch (Exception e)
            {
                Utilities.Logger(e.Message);
            }
        }

        protected void Enable()
        {
            Drawing.OnDraw += OnDrawingDraw;
        }

        protected void Disable()
        {
            Drawing.OnDraw -= OnDrawingDraw;
        }

        private void OnDrawingDraw(EventArgs args)
        {
            try
            {
                if (!Utilities.MainMenu["CLOCK"]["ShowTime"])
                {
                    Disable();
                    return;
                }
                
                var screen = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
                var width = (float)screen.Width;
                Drawing.DrawText(width - 100, 100, Color.Crimson, DateTime.Now.ToShortTimeString());
            }
            catch (Exception e)
            {
                Utilities.Logger(e.Message);
            }
        }

    }

}

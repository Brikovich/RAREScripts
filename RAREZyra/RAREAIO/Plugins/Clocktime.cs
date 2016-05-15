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

        public bool IsRunning { private set; get; }

        public void Init()
        {
            OnLoad();
        }
        
        protected void OnLoad()
        {
            try
            {
                Bootstrap.ClockMenu.Bool("ShowTime", "Show time");
                Bootstrap.ClockMenu.Slider("WidthOffset", "Width offset", 0, -100);
                Bootstrap.ClockMenu.Slider("HeightOffset", "height offset", 0, -100);
                Bootstrap.ClockMenu.Add(new Menu())
                Drawing.OnDraw += OnDrawingDraw;
                IsRunning = true;
            }
            catch (Exception e)
            {
                Utilities.Logger(e.Message);
            }
        }

        internal void Enable()
        {
            Drawing.OnDraw += OnDrawingDraw;
            IsRunning = true;
        }

        internal void Disable()
        {
            Drawing.OnDraw -= OnDrawingDraw;
            IsRunning = false;
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
                Drawing.DrawText(width - 100 + Utilities.MainMenu["CLOCK"]["WidthOffset"], 100 + Utilities.MainMenu["CLOCK"]["HeightOffset"], Color.Azure, DateTime.Now.ToShortTimeString());
            }
            catch (Exception e)
            {
                Utilities.Logger(e.Message);
            }
        }

    }

}

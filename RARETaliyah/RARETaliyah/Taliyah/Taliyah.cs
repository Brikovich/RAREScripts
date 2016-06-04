using System;
using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.Enumerations;
using LeagueSharp.SDK.Utils;
using Color = System.Drawing.Color;


namespace RARETaliyah.Taliyah
{
    internal class Taliyah
    {

        private readonly SpellQ _q;
        private readonly SpellW _w;
        private readonly SpellE _e;
        private readonly SpellR _r;
        private readonly Passive _p;

        public Taliyah()
        {
            
            _q = new SpellQ(new Spell(SpellSlot.Q, 100).SetSkillshot(0f, 20f, float.MaxValue, true, SkillshotType.SkillshotLine));
            _e = new SpellE(new Spell(SpellSlot.E, 100).SetSkillshot(0.5f, 100f, float.MaxValue, false, SkillshotType.SkillshotCircle));
            _w = new SpellW(new Spell(SpellSlot.W, 500f).SetSkillshot(0.5f, 500f, float.MaxValue, false, SkillshotType.SkillshotCone));
            _r = new SpellR(new Spell(SpellSlot.R, 3000f).SetSkillshot(0f, 50f, float.MaxValue, false, SkillshotType.SkillshotLine));
            _p = new Passive(500);

        }

        internal void Drawing_OnDraw(EventArgs args)
        {
            if (_q.SpellDetails.Level >= 1)
                Render.Circle.DrawCircle(GameObjects.Player.Position, _q.SpellDetails.Range, Color.Aqua);
            if (_w.SpellDetails.Level >= 1)
                Render.Circle.DrawCircle(GameObjects.Player.Position, _w.SpellDetails.Range, Color.DarkGoldenrod);
            if (_e.SpellDetails.Level >= 1)
                Render.Circle.DrawCircle(GameObjects.Player.Position, _e.SpellDetails.Range, Color.Firebrick);
            if (_r.SpellDetails.Level >= 1)
                Render.Circle.DrawCircle(GameObjects.Player.Position, _r.SpellDetails.Range, Color.Indigo);
        }

        internal void Game_OnUpdate(EventArgs args)
        {
            
        }


        
    }
}
